export const thingTest = async () => {
    const thingPromise = await fetch("http://localhost:5065");
    const thingObj = await thingPromise.text();
    return thingObj;
};
export const ThingTest2 = () => {
    return "I am a thing2";
};
//# sourceMappingURL=service.js.map