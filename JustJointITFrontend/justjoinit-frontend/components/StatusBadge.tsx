import { PromptStatus } from "@/lib/types";

const STATUS_LABEL: Record<PromptStatus, string> = {
  [PromptStatus.Unknown]: "Unknown",
  [PromptStatus.Pending]: "Pending",
  [PromptStatus.Processing]: "Processing",
  [PromptStatus.Completed]: "Completed",
  [PromptStatus.Failed]: "Failed",
};

const spinning = (
  <span className="inline-block h-3 w-3 animate-spin rounded-full border-2 border-current border-t-transparent" />
);

const base =
  "inline-flex items-center gap-1.5 rounded-full px-2.5 py-0.5 text-xs font-medium";

export default function StatusBadge({ status }: { status: PromptStatus }) {
  switch (status) {
    case PromptStatus.Pending:
      return (
        <span className={`${base} bg-yellow-100 text-yellow-800`}>
          {spinning} Pending
        </span>
      );
    case PromptStatus.Processing:
      return (
        <span className={`${base} bg-blue-100 text-blue-800`}>
          {spinning} Processing
        </span>
      );
    case PromptStatus.Completed:
      return (
        <span className={`${base} bg-green-100 text-green-800`}>
          <span>✓</span> Completed
        </span>
      );
    case PromptStatus.Failed:
      return (
        <span className={`${base} bg-red-100 text-red-800`}>
          <span>✗</span> Failed
        </span>
      );
    default:
      return (
        <span className={`${base} bg-zinc-100 text-zinc-600`}>
          {STATUS_LABEL[PromptStatus.Unknown]}
        </span>
      );
  }
}
