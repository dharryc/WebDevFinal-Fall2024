import { validateUser } from "./loginService.js";
const logInNode = document.getElementById("userLogin");

logInNode?.addEventListener("submit", async (ev) => {
  ev.preventDefault();
  const username = (
    document.getElementById("username") as any
  ).value.toLowerCase();
  if (await validateUser(username)) {
    window.location.href = `https://dharryc.github.io/WebDevFinal-Fall2024/userListPagePrototype.html?user=${username}`;
  } else {
    const errorText = document.getElementById("error") as any;
    errorText.textContent = "I couldn't find that user.";
  }
});
