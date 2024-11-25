export const currentUser = async (userName) => {
    const userPromise = await fetch(`http://localhost:5065/user/${userName}`);
    const userObj = await userPromise.json();
    return userObj;
};
//# sourceMappingURL=service.js.map