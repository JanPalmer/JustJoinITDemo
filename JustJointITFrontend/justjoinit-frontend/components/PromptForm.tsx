"use client";

import { Model } from "@/lib/types";

interface PromptFormProps {
  models: Model[];
  loadingModels: boolean;
  selectedModelId: number | "";
  content: string;
  submitting: boolean;
  error: string | null;
  onModelChange: (id: number) => void;
  onContentChange: (value: string) => void;
  onSubmit: (e: React.FormEvent) => void;
}

export default function PromptForm({
  models,
  loadingModels,
  selectedModelId,
  content,
  submitting,
  error,
  onModelChange,
  onContentChange,
  onSubmit,
}: PromptFormProps) {
  return (
    <section className="rounded-lg border border-zinc-200 bg-white p-6 shadow-sm">
      <h2 className="mb-4 text-sm font-medium text-zinc-700">New Prompt</h2>
      <form onSubmit={onSubmit} className="space-y-4">
        <div>
          <label
            htmlFor="model"
            className="block text-xs font-medium text-zinc-600 mb-1"
          >
            Model
          </label>
          {loadingModels ? (
            <div className="h-9 w-full animate-pulse rounded-md bg-zinc-100" />
          ) : (
            <select
              id="model"
              value={selectedModelId}
              onChange={(e) => onModelChange(Number(e.target.value))}
              disabled={models.length === 0}
              className="w-full rounded-md border border-zinc-300 bg-white px-3 py-2 text-sm text-zinc-800 focus:outline-none focus:ring-2 focus:ring-blue-500 disabled:opacity-50"
            >
              {models.map((m) => (
                <option key={m.id} value={m.id}>
                  {m.name} ({m.family})
                </option>
              ))}
            </select>
          )}
        </div>

        <div>
          <label
            htmlFor="prompt"
            className="block text-xs font-medium text-zinc-600 mb-1"
          >
            Prompt
          </label>
          <textarea
            id="prompt"
            value={content}
            onChange={(e) => onContentChange(e.target.value)}
            rows={5}
            placeholder="Enter your prompt here…"
            className="w-full rounded-md border border-zinc-300 px-3 py-2 text-sm text-zinc-800 placeholder-zinc-400 focus:outline-none focus:ring-2 focus:ring-blue-500 resize-none"
          />
        </div>

        {error && <p className="text-xs text-red-600">{error}</p>}

        <button
          type="submit"
          disabled={submitting || !content.trim() || !selectedModelId}
          className="w-full rounded-md bg-blue-600 px-4 py-2 text-sm font-medium text-white hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
        >
          {submitting ? "Submitting…" : "Submit"}
        </button>
      </form>
    </section>
  );
}
