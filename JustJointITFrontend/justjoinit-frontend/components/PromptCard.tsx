import { Prompt, PromptStatus } from "@/lib/types";
import StatusBadge from "@/components/StatusBadge";

export default function PromptCard({ prompt }: { prompt: Prompt }) {
    switch (prompt.status) {
      case PromptStatus.Completed:
        return (
          <div
            className="rounded-lg border p-4 border-green-200 bg-green-50">
            <div className="mb-2 flex items-start justify-between gap-2">
              <span className="text-xs font-medium text-zinc-500">
                {prompt.modelName}
              </span>
              <StatusBadge status={prompt.status} />
            </div>
            <p className="mb-3 text-sm text-zinc-800 whitespace-pre-wrap break-words">
              {prompt.content}
            </p>
            <div className="rounded border border-green-300 bg-white p-3">
              <p className="text-xs font-medium text-green-700 mb-1">Response</p>
                <p className="text-sm text-zinc-800 whitespace-pre-wrap break-words">
                  {prompt.result}
                </p>
            </div>
          </div>
        );
      case PromptStatus.Failed:
        return (
          <div
            className="rounded-lg border p-4 border-red-200 bg-red-50">
            <div className="mb-2 flex items-start justify-between gap-2">
              <span className="text-xs font-medium text-zinc-500">
                {prompt.modelName}
              </span>
              <StatusBadge status={prompt.status} />
            </div>
            <p className="mb-3 text-sm text-zinc-800 whitespace-pre-wrap break-words">
              {prompt.content}
            </p>            
            <div className="rounded border border-red-300 bg-white p-3">
              <p className="text-xs font-medium text-red-700 mb-1">Error</p>
                <p className="text-sm text-zinc-800 whitespace-pre-wrap break-words">
                  {prompt.errorMessage}
                </p>
            </div>
          </div>
        );
        case PromptStatus.Pending:
        case PromptStatus.Processing:
        default:
          return (
            <div
              className="rounded-lg border p-4 border-blue-200 bg-blue-50">
              <div className="mb-2 flex items-start justify-between gap-2">
                <span className="text-xs font-medium text-zinc-500">
                  {prompt.modelName}
                </span>
                <StatusBadge status={prompt.status} />
              </div>
            </div>
          );
      }
}
