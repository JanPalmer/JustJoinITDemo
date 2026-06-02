import { Model, Prompt, AddPromptRequest } from "@/app/lib/types";

export async function getModels(): Promise<Model[]> {
  const res = await fetch("/api/models");
  if (!res.ok) throw new Error("Failed to load models");
  return res.json();
}

export async function getAllPrompts(): Promise<Prompt[]> {
  const res = await fetch("/api/prompts");
  if (!res.ok) return [];
  return res.json();
}

export async function getPrompt(id: number): Promise<Prompt> {
  const res = await fetch(`/api/prompts/${id}`);
  if (!res.ok) throw new Error(`Failed to fetch prompt ${id}`);
  return res.json();
}

export async function submitPrompt(req: AddPromptRequest): Promise<Prompt> {
  const res = await fetch("/api/prompts", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(req),
  });
  if (!res.ok) throw new Error("Failed to submit prompt");
  return res.json();
}
