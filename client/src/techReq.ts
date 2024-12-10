const formNode: any = document.getElementById("formReqs");
const formP: any = document.getElementById("dumpInput");
const textInput: any = document.getElementById("text");
const numberInput: any = document.getElementById("number");
const selectInput: any = document.getElementById("select");
const deleteInput: any = document.getElementById("reset");

deleteInput.addEventListener("click", () => {
  formP.textContent =
    "Here's where you'll be able to see everything when you submit it! (as in literally here, like, I'm going to replace this text with all of your inputs)";
});

formNode.addEventListener("submit", (ev: { preventDefault: () => void }) => {
  ev.preventDefault();
  formP.textContent =
    textInput.value + " " + numberInput.value + " " + selectInput.value;
});

//Drag and drop stuff
const draggableNode = document.getElementById("thingToDragNDrop");
draggableNode?.setAttribute("draggable", "true");

const duplicateNode = document.getElementById("duplicateIt");
duplicateNode?.addEventListener("dragover", (ev) => {
  ev.preventDefault();
});
duplicateNode?.addEventListener("drop", () => {
  duplicateNode.append(makeCopyNode());
  console.log("in here");
});

const makeCopyNode = () => {
  const id = Date.now();
  const nodeToReturn = document.createElement("div");
  nodeToReturn.classList.add("thingToDragNDrop");
  nodeToReturn.textContent = "Here's the thing" + "\n And here's its ID: " + id;
  nodeToReturn.setAttribute("draggable", "true");
  nodeToReturn.setAttribute("id", id.toString());
  nodeToReturn.addEventListener("dragstart", (ev) => {
    ev.dataTransfer?.setData("id", id.toString());
  });
  return nodeToReturn;
};

const deleteNode = document.getElementById("deleteIt");
deleteNode?.addEventListener("dragover", (ev) => {
  ev.preventDefault();
});
deleteNode?.addEventListener("drop", (ev) => {
  const thing1: any = ev.dataTransfer?.getData("id");
  const thing: any = document.getElementById(thing1);
  duplicateNode?.removeChild(thing);
});
