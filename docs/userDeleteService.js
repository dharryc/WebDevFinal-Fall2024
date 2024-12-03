export const DeleteUser = async (username) => {
    await fetch(`https://finalproject-35asp3tk.b4a.run/user/${username}/delete`, {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json",
        },
    });
};
//# sourceMappingURL=userDeleteService.js.map