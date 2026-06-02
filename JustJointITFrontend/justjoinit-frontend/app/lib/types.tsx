export type Model = {
  id: number;
  name: string;
  family: string;
}

export type Prompt = {
  id: number;
  modelName: string;
  content: string;
  status: PromptStatus;
  result?: string;
  errorMessage?: string;
}

export enum PromptStatus {
  Unknown = 0,
  Pending = 1,
  Processing = 2,
  Completed = 3,
  Failed = 4,
}


export type AddPromptRequest = {
  modelId: number;
  content: string;
}
