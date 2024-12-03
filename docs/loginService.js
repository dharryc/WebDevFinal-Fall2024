export const validateUser = async (user) => {
    const users = await fetch("http://localhost:5065/userList");
    const usersObj = await users.json();
    if (usersObj.includes(user)) {
        return true;
    }
    return false;
};
//# sourceMappingURL=loginService.js.map