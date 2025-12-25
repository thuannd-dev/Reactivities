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
  //input type of all z.coerce schemas is unknown by default.
  //To specify a more specific input type, pass a generic parameter
  date: z.coerce
    .date<Date>({ error: "Date is required" })
    .min(new Date(), { error: "Date must be in the future" }),
  //refine is a custom validation, you mustreturn a falsy value to signal failure
  location: z.object(
    {
      venue: requiredString("Venue"),
      city: z.string().optional(),
      latitude: z.coerce.number<number>(),
      longitude: z.coerce.number<number>(),
    },
    {
      error: "Please type and select the location from the list",
    }
  ),
  // .refine((val) => typeof val !== "string", {
  //   error: "Please type and select the location from the list",
  // }),
});
// export type ActivitySchema = z.input<typeof activitySchema>;
export type ActivitySchema = z.infer<typeof activitySchema>;
//input is the type before parsing, type user provide
//infer is the type after parsing, validate / coerce, this is the type we usually use in applications
