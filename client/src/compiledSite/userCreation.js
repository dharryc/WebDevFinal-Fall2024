// public record User(string UserName, string[]? Links, string[]? Descriptions, ulong Id);
import { userNameVerification } from "./userCreationService.js";
const makeForm = () => {
    const userCreationForm = document.getElementById("userForm");
    const formWrapperNode = document.createElement("form");
    formWrapperNode.setAttribute("action", "submit");
    formWrapperNode.setAttribute("id", "newUserForm");
    const userNameLabel = document.createElement("label");
    userNameLabel.setAttribute("for", "userName");
    userNameLabel.textContent = "Username:";
    const userNameInput = document.createElement("input");
    userNameInput.setAttribute("type", "text");
    userNameInput.setAttribute("id", "userName");
    userNameInput.setAttribute("name", "userName");
    const submitButton = document.createElement("input");
    submitButton.setAttribute("type", "submit");
    submitButton.setAttribute("value", "Create User");
    formWrapperNode.addEventListener("submit", async (ev) => {
        ev.preventDefault();
        await userNameVerification(userNameInput.value);
    });
    formWrapperNode.append(userNameLabel, userNameInput, submitButton);
    userCreationForm?.append(formWrapperNode);
};
makeForm();
//# sourceMappingURL=userCreation.js.map