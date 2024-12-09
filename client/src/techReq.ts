const formNode: any = document.getElementById("formReqs");
const formP: any = document.getElementById("dumpInput");
const textInput: any = document.getElementById("text");
const numberInput: any = document.getElementById("number");
const selectInput: any = document.getElementById("select");

formNode.addEventListener("submit", (ev: { preventDefault: () => void }) => {
  ev.preventDefault();
  formP.textContent = textInput.value + " " + numberInput.value + " " + selectInput.value;
});
