import * as path from "node:path";
import * as process from "node:process";
import { generateApi } from "swagger-typescript-api";

try {
  await generateApi({
    fileName: "api.ts",
    url: "http://localhost:7000/openapi/v1.json",
    output: path.join(process.cwd(), "e2e"),
    httpClientType: "axios",
    extractRequestParams: true,
  });

  console.log("API generated successfully!");
} catch (error) {
  console.error("Error generating API:", error);
}
