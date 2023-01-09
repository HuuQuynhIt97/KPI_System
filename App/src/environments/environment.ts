import { commonPerFactory } from "src/app/_core/_helper/common-per-factory";

export const environment = {
  production: false,
  enableDebug: true,
  systemCode: commonPerFactory.systemCode,
  apiUrlEC: commonPerFactory.apiUrlEC,
  apiUrl: commonPerFactory.apiUrl,
  hub: commonPerFactory.hub,
  scalingHub: commonPerFactory.scalingHub,
  scalingHubLocal: commonPerFactory.scalingHubLocal,
  allowedDomains: commonPerFactory.allowedDomains,
  disallowedRoutes: commonPerFactory.disallowedRoutes,
};