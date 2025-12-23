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
  //input type of all z.coerce schemas is unknown
  date: z.coerce.date({ error: "Date is required" }),
  location: z.object({
    venue: requiredString("Venue"),
    city: z.string().optional(),
    latitude: z.coerce.number(),
    longitude: z.coerce.number(),
  }),
});
//change to input type from infer to get the schema the correct types for form input values
//if don't use coerce, i prefer using z.infer
export type ActivitySchema = z.input<typeof activitySchema>;
