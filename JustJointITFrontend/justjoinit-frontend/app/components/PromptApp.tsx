"use client";

import { useState, useEffect } from "react";
import { Model, Prompt, PromptStatus } from "@/app/lib/types";
import { getModels, getAllPrompts, submitPrompt } from "@/app/lib/api";
import { usePolling } from "@/app/hooks/usePolling";
import PromptForm from "@/app/components/PromptForm";
import PromptCard from "@/app/components/PromptCard";

export default function PromptApp() {
  const [models, setModels] = useState<Model[]>([]);
  const [selectedModelId, setSelectedModelId] = useState<number | "">("");
  const [content, setContent] = useState("");
  const [submitting, setSubmitting] = useState(false);
  const [loadError, setLoadError] = useState<string | null>(null);
  const [submitError, setSubmitError] = useState<string | null>(null);
  const [prompts, setPrompts] = useState<Prompt[]>([]);
  const [loadingModels, setLoadingModels] = useState(true);

  usePolling(prompts, setPrompts);

  useEffect(() => {
    Promise.all([getModels(), getAllPrompts()])
      .then(([modelData, promptData]) => {
        setModels(modelData);
        if (modelData.length > 0)
        {
          setSelectedModelId(modelData[0].id);
        }
        setPrompts(promptData);
      })
      .catch(() =>
        setLoadError("Could not connect to the backend. Is it running on port 5161?")
      )
      .finally(() => setLoadingModels(false));
  }, []);

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    if (!selectedModelId || !content.trim()) return;
    setSubmitting(true);
    setSubmitError(null);
    try {
      const newPrompt = await submitPrompt({ modelId: selectedModelId, content: content.trim() });
      setPrompts((prev) => [newPrompt, ...prev]);
      setContent("");
    } catch {
      setSubmitError("Failed to submit prompt. Please try again.");
    } finally {
      setSubmitting(false);
    }
  }

  const pendingCount = prompts.filter(
    (p) =>
      p.status === PromptStatus.Pending ||
      p.status === PromptStatus.Processing
  ).length;

  return (
    <div className="min-h-screen bg-zinc-50">
      <header className="border-b border-zinc-200 bg-white px-6 py-4">
        <h1 className="text-lg font-semibold text-zinc-900">LLM Prompt Tool</h1>
        {pendingCount > 0 && (
          <p className="text-xs text-blue-600 mt-0.5">
            {pendingCount} prompt{pendingCount > 1 ? "s" : ""} processing…
          </p>
        )}
      </header>

      <main className="mx-auto max-w-2xl px-4 py-8 space-y-6">
        {loadError && (
          <div className="rounded-lg border border-red-200 bg-red-50 p-4 text-sm text-red-700">
            {loadError}
          </div>
        )}

        <PromptForm
          models={models}
          loadingModels={loadingModels}
          selectedModelId={selectedModelId}
          content={content}
          submitting={submitting}
          error={submitError}
          onModelChange={setSelectedModelId}
          onContentChange={setContent}
          onSubmit={handleSubmit}
        />

        {prompts.length > 0 && (
          <section>
            <div className="space-y-3">
              {prompts.map((p) => (
                <PromptCard key={p.id} prompt={p} />
              ))}
            </div>
          </section>
        )}

        {!loadingModels && prompts.length === 0 && !loadError && (
          <p className="text-center text-sm text-zinc-400">
            No prompts yet. Submit one above.
          </p>
        )}
      </main>
    </div>
  );
}
