import { useEffect, useRef } from "react";
import { Prompt, PromptStatus } from "@/lib/types";
import { getPrompt } from "@/lib/api";

const POLL_INTERVAL_MS = 2000;

export function usePolling(
  prompts: Prompt[],
  setPrompts: React.Dispatch<React.SetStateAction<Prompt[]>>
) {
  const promptsRef = useRef(prompts);
  promptsRef.current = prompts;

  useEffect(() => {
    const hasPending = prompts.some(
      (p) =>
        p.status === PromptStatus.Pending ||
        p.status === PromptStatus.Processing
    );

    if (!hasPending) return;

    const interval = setInterval(async () => {
      const pending = promptsRef.current.filter(
        (p) =>
          p.status === PromptStatus.Pending ||
          p.status === PromptStatus.Processing
      );

      const updated = await Promise.all(
        pending.map(async (p) => {
          try {
            return await getPrompt(p.id);
          } catch {
            return p;
          }
        })
      );

      setPrompts((prev) =>
        prev.map((p) => updated.find((u: Prompt) => u.id === p.id) ?? p)
      );
    }, POLL_INTERVAL_MS);

    return () => clearInterval(interval);
  }, [prompts, setPrompts]);
}
