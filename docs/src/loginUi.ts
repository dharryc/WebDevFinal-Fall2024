import { validateUser } from "./loginService.js";
const logInNode = document.getElementById("userLogin");

logInNode?.addEventListener("submit", async (ev) => {
  ev.preventDefault();
  const username = (
    document.getElementById("username") as any
  ).value.toLowerCase();
  if (await validateUser(username)) {
    window.location.href = `http://127.0.0.1:5500/compiledSite/userListPagePrototype.html?user=${username}`;
  }
  const errorText = (document.getElementById("error") as any)
  errorText.textContent = "I couldn't find that user."
});
