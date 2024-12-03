export const makeUser = async (user) => {
    await fetch("https://finalproject-35asp3tk.b4a.run/newUser", {
        method: "POST",
        body: JSON.stringify(user),
        headers: {
            "Content-Type": "application/json",
        },
    });
};
export const userNameVerification = async (userName) => {
    const userNameList = await fetch("https://finalproject-35asp3tk.b4a.run/userList");
    const userNameObj = await userNameList.json();
    if (userNameObj.includes(userName.toLocaleLowerCase())) {
        return "That username already exists. Please contact Harry if the site is broken, or log in again with the same name";
    }
    else {
        await makeUser({ userName: userName.toLowerCase(), id: Date.now() });
        return "User successfully created";
    }
};
//# sourceMappingURL=userCreationService.js.map