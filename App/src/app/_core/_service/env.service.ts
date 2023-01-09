import { environment } from "src/environments/environment";

export class EnvService {

  // API url
  public apiUrl = environment.apiUrl;

  // Whether or not to enable debug mode
  public enableDebug = environment.enableDebug;
}
