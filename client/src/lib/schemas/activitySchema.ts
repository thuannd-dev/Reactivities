import { z } from "zod";

const requiredString = (fieldName: string) =>
  z
    .string({
      error: (issue) =>
        issue.input === undefined ? `${fieldName} is required` : "Not a string",
    })
    .trim()
    .min(1, { error: `${fieldName} is required` });
//Just when using controll input (controll input because using useController to create reusable input) you must need
//{
//   error: (issue) =>
//     issue.input === undefined ? `${fieldName} is required` : "Not a string",
// }
// to customize message for undefined input - default message is "Expected string, received undefined"

//Zod will convert the input value to a string before validating, because HTML input always returns a string.
//=> Input 123 not going to fail "not a string" error

export const activitySchema = z.object({
  title: requiredString("Title"),
  description: requiredString("Description"),
  category: requiredString("Category"),
  date: requiredString("Date"),
  city: requiredString("City"),
  venue: requiredString("Venue"),
});

export type ActivitySchema = z.infer<typeof activitySchema>;
