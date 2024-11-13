export const thingTest = async () => {
  const thingPromise = await fetch("http://localhost:5065");
  const thingObj = await thingPromise.text();
  return thingObj;
};
