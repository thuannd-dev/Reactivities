import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import type { LoginSchema } from "../schemas/loginSchema";
import agent from "../api/agent";

export const useAccount = () => {
  const queryClient = useQueryClient();

  //After user logs in i want to fecth user info to know user already logged in successfully or not
  //Because i can't access to cookies from client side (httpOnly cookies)
  const loginUser = useMutation({
    mutationFn: async (creds: LoginSchema) => {
      await agent.post("/login?useCookies=true", creds);
    },
    onSuccess: async () => {
      //Invaidate user query that force React Query to refetch user info
      await queryClient.invalidateQueries({
        queryKey: ["user"],
      });
    },
  });

  const { data: currentUser } = useQuery({
    queryKey: ["user"],
    queryFn: async () => {
      const response = await agent.get<User>("/account/user-info");
      return response.data;
    },
  });

  return { loginUser, currentUser };
};
