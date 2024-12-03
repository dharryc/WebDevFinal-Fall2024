export const DeleteUser = async (username) => {
    await fetch(`http://localhost:5065/user/${username}/delete`, {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json",
        },
    });
};
//# sourceMappingURL=userDeleteService.js.map