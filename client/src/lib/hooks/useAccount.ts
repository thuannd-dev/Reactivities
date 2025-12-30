import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import type { LoginSchema } from "../schemas/loginSchema";
import agent from "../api/agent";
import { useLocation, useNavigate } from "react-router";
import type { RegisterSchema } from "../schemas/registerSchema";
import { toast } from "react-toastify";

export const useAccount = () => {
  const queryClient = useQueryClient();
  const location = useLocation();
  const navigate = useNavigate();

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

  const registerUser = useMutation({
    mutationFn: async (creds: RegisterSchema) => {
      await agent.post("/account/register", creds);
    },
    onSuccess: () => {
      toast.success("Register successful - you can now login");
      navigate("/login");
    },
  });

  const logoutUser = useMutation({
    mutationFn: async () => {
      await agent.post("/account/logout");
    },
    onSuccess: () => {
      queryClient.removeQueries({ queryKey: ["user"] });
      queryClient.removeQueries({ queryKey: ["activities"] });
      navigate("/");
    },
  });

  //if we don't set the query to enabled -> default value is true
  //  it will re run hook (re run useQuery) every time component re renders, every time useAccount is called
  //And this may be go to api and fetch user info again and again if state of query is stale
  // we don't want that because we just want to fetch user info once when app loads
  const { data: currentUser, isLoading: loadingUserInfo } = useQuery({
    queryKey: ["user"],
    queryFn: async () => {
      const response = await agent.get<User>("/account/user-info");
      return response.data;
    },
    //it mean that if we already have user data in cache we don't need to run this query again and the path is not /register
    enabled:
      !queryClient.getQueryData(["user"]) && location.pathname !== "/register",
    // && location.pathname !== "/login"
    //When user login, we need to fetch user info and store it in state,
    //  if disable this, <RequireAuth> will redirect user to login page forever
    //  instead of letting user access /activities page
  });

  return { loginUser, registerUser, currentUser, logoutUser, loadingUserInfo };
};
