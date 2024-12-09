"use strict";
const formNode = document.getElementById("formReqs");
const formP = document.getElementById("dumpInput");
const textInput = document.getElementById("text");
const numberInput = document.getElementById("number");
const selectInput = document.getElementById("select");
formNode.addEventListener("submit", (ev) => {
    ev.preventDefault();
    formP.textContent = textInput.value + " " + numberInput.value + " " + selectInput.value;
});
//# sourceMappingURL=techReq.js.map