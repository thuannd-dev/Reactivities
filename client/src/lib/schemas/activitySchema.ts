import { z } from "zod";

export const activitySchema = z.object({
  title: z
    .string({
      error: (issue) =>
        issue.input === undefined ? "Title is required" : "Not a string",
    })
    .min(1, { error: "Title is required" }),
});

export type ActivitySchema = z.infer<typeof activitySchema>;
