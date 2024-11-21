export const makeUser = async (user) => {
    await fetch("http://localhost:5065/newUser", {
        method: "POST",
        body: JSON.stringify(user),
        headers: {
            "Content-Type": "application/json",
        },
    });
};
export const userNameVerification = async (userName) => {
    const userNameList = await fetch("http://localhost:5065/userList");
    const userNameObj = await userNameList.json();
    if (userNameObj.includes(userName.toLocaleLowerCase())) {
        return "That username is already used. Please contact Harry if the site is broken, or log in again with the same name";
    }
    else {
        await makeUser({ userName: userName.toLowerCase() });
    }
};
//# sourceMappingURL=userCreationService.js.map