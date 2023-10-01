import { UserModel } from "./models/userModel";

const currentUserLoginKey = 'login';

export const CurrentUSer = {
    login: localStorage.getItem(currentUserLoginKey),

    setCurrentUaer: (userModel: UserModel) => {
        //localStorage.setItem(currentUserLoginKey)
    }
}