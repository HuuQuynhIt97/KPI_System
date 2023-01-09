// const port = 5000; // LOCALHOST
 //const port = 1202; // SHC TEST
const port = 1200; // SHC, CB, TSH, SPC
const ip = `${window.location.hostname}:${port}`;
const apiUrl = `http://${ip}`;
const systemCode = 3;

export const commonPerFactory = {
  systemCode: systemCode,
  apiUrlEC: `${apiUrl}/api/`,
  apiUrl: `${apiUrl}/api/`,
  hub: `${apiUrl}/kpi-hub`,
  scalingHub: `${apiUrl}/ec-hub`,
  scalingHubLocal: `${apiUrl}/scalingHub`,
  allowedDomains: [ip],
  disallowedRoutes: [`${ip}/api/auth`]
}
