export const testApi = async () => {
  const thing = await fetch("http://localhost:5065");
  const thingObj = await thing.text();
  return thingObj;
};
