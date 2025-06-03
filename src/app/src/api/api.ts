/* eslint-disable */
/* tslint:disable */
// @ts-nocheck
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

export enum FlashcardSource {
  Manual = "Manual",
  AIFull = "AIFull",
  AIEdited = "AIEdited",
}

export interface CreateFlashcardData {
  front?: string | null;
  back?: string | null;
  source?: FlashcardSource;
  /** @format int64 */
  generationId?: number | null;
}

export interface CreateFlashcardsRequest {
  flashcards?: CreateFlashcardData[] | null;
}

export interface Error {
  code: string;
  message: string;
  /** @default null */
  propertyName?: string | null;
}

export interface FlashcardsListItem {
  /** @format int64 */
  id: number;
  front: string;
  back: string;
}

export interface GeneratedFlashcard {
  front: string;
  back: string;
}

export interface GenerateFlashcardsRequest {
  sourceText?: string | null;
}

export type GenerateFlashcardsResponse = {
  flashcards: GeneratedFlashcard[];
  /** @format int64 */
  generationId: number;
} | null;

export type GetFlashcardsResponse = {
  items: FlashcardsListItem[];
  /** @format int32 */
  total: number;
} | null;

export interface Response {
  isSuccess: boolean;
  errors: Error[];
}

export interface ResponseOfGenerateFlashcardsResponse {
  data?: GenerateFlashcardsResponse;
  isSuccess: boolean;
  errors: Error[];
}

export interface ResponseOfGetFlashcardsResponse {
  data?: GetFlashcardsResponse;
  isSuccess: boolean;
  errors: Error[];
}

export interface UpdateFlashcardRequest {
  front?: string | null;
  back?: string | null;
}

export interface GetFlashcardsParams {
  /** @format int32 */
  PageNumber?: number;
  /** @format int32 */
  PageSize?: number;
}

import type {
  AxiosInstance,
  AxiosRequestConfig,
  AxiosResponse,
  HeadersDefaults,
  ResponseType,
} from "axios";
import axios from "axios";

export type QueryParamsType = Record<string | number, any>;

export interface FullRequestParams
  extends Omit<AxiosRequestConfig, "data" | "params" | "url" | "responseType"> {
  /** set parameter to `true` for call `securityWorker` for this request */
  secure?: boolean;
  /** request path */
  path: string;
  /** content type of request body */
  type?: ContentType;
  /** query params */
  query?: QueryParamsType;
  /** format of response (i.e. response.json() -> format: "json") */
  format?: ResponseType;
  /** request body */
  body?: unknown;
}

export type RequestParams = Omit<
  FullRequestParams,
  "body" | "method" | "query" | "path"
>;

export interface ApiConfig<SecurityDataType = unknown>
  extends Omit<AxiosRequestConfig, "data" | "cancelToken"> {
  securityWorker?: (
    securityData: SecurityDataType | null,
  ) => Promise<AxiosRequestConfig | void> | AxiosRequestConfig | void;
  secure?: boolean;
  format?: ResponseType;
}

export enum ContentType {
  Json = "application/json",
  FormData = "multipart/form-data",
  UrlEncoded = "application/x-www-form-urlencoded",
  Text = "text/plain",
}

export class HttpClient<SecurityDataType = unknown> {
  public instance: AxiosInstance;
  private securityData: SecurityDataType | null = null;
  private securityWorker?: ApiConfig<SecurityDataType>["securityWorker"];
  private secure?: boolean;
  private format?: ResponseType;

  constructor({
    securityWorker,
    secure,
    format,
    ...axiosConfig
  }: ApiConfig<SecurityDataType> = {}) {
    this.instance = axios.create({
      ...axiosConfig,
      baseURL: axiosConfig.baseURL || "http://localhost:7000",
    });
    this.secure = secure;
    this.format = format;
    this.securityWorker = securityWorker;
  }

  public setSecurityData = (data: SecurityDataType | null) => {
    this.securityData = data;
  };

  protected mergeRequestParams(
    params1: AxiosRequestConfig,
    params2?: AxiosRequestConfig,
  ): AxiosRequestConfig {
    const method = params1.method || (params2 && params2.method);

    return {
      ...this.instance.defaults,
      ...params1,
      ...(params2 || {}),
      headers: {
        ...((method &&
          this.instance.defaults.headers[
            method.toLowerCase() as keyof HeadersDefaults
          ]) ||
          {}),
        ...(params1.headers || {}),
        ...((params2 && params2.headers) || {}),
      },
    };
  }

  protected stringifyFormItem(formItem: unknown) {
    if (typeof formItem === "object" && formItem !== null) {
      return JSON.stringify(formItem);
    } else {
      return `${formItem}`;
    }
  }

  protected createFormData(input: Record<string, unknown>): FormData {
    if (input instanceof FormData) {
      return input;
    }
    return Object.keys(input || {}).reduce((formData, key) => {
      const property = input[key];
      const propertyContent: any[] =
        property instanceof Array ? property : [property];

      for (const formItem of propertyContent) {
        const isFileType = formItem instanceof Blob || formItem instanceof File;
        formData.append(
          key,
          isFileType ? formItem : this.stringifyFormItem(formItem),
        );
      }

      return formData;
    }, new FormData());
  }

  public request = async <T = any, _E = any>({
    secure,
    path,
    type,
    query,
    format,
    body,
    ...params
  }: FullRequestParams): Promise<AxiosResponse<T>> => {
    const secureParams =
      ((typeof secure === "boolean" ? secure : this.secure) &&
        this.securityWorker &&
        (await this.securityWorker(this.securityData))) ||
      {};
    const requestParams = this.mergeRequestParams(params, secureParams);
    const responseFormat = format || this.format || undefined;

    if (
      type === ContentType.FormData &&
      body &&
      body !== null &&
      typeof body === "object"
    ) {
      body = this.createFormData(body as Record<string, unknown>);
    }

    if (
      type === ContentType.Text &&
      body &&
      body !== null &&
      typeof body !== "string"
    ) {
      body = JSON.stringify(body);
    }

    return this.instance.request({
      ...requestParams,
      headers: {
        ...(requestParams.headers || {}),
        ...(type ? { "Content-Type": type } : {}),
      },
      params: query,
      responseType: responseFormat,
      data: body,
      url: path,
    });
  };
}

/**
 * @title Memoraid.WebApi | v1
 * @version 1.0.0
 * @baseUrl http://localhost:7000
 */
export class Api<
  SecurityDataType extends unknown,
> extends HttpClient<SecurityDataType> {
  flashcards = {
    /**
     * No description
     *
     * @tags Memoraid.WebApi
     * @name GenerateFlashcards
     * @request POST:/flashcards/generate
     */
    generateFlashcards: (
      data: GenerateFlashcardsRequest,
      params: RequestParams = {},
    ) =>
      this.request<ResponseOfGenerateFlashcardsResponse, any>({
        path: `/flashcards/generate`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Memoraid.WebApi
     * @name CreateFlashcards
     * @request POST:/flashcards
     */
    createFlashcards: (
      data: CreateFlashcardsRequest,
      params: RequestParams = {},
    ) =>
      this.request<Response, any>({
        path: `/flashcards`,
        method: "POST",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Memoraid.WebApi
     * @name GetFlashcards
     * @request GET:/flashcards
     */
    getFlashcards: (query: GetFlashcardsParams, params: RequestParams = {}) =>
      this.request<ResponseOfGetFlashcardsResponse, any>({
        path: `/flashcards`,
        method: "GET",
        query: query,
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Memoraid.WebApi
     * @name DeleteFlashcard
     * @request DELETE:/flashcards/{id}
     */
    deleteFlashcard: (id: number, params: RequestParams = {}) =>
      this.request<Response, any>({
        path: `/flashcards/${id}`,
        method: "DELETE",
        format: "json",
        ...params,
      }),

    /**
     * No description
     *
     * @tags Memoraid.WebApi
     * @name UpdateFlashcard
     * @request PUT:/flashcards/{id}
     */
    updateFlashcard: (
      id: number,
      data: UpdateFlashcardRequest,
      params: RequestParams = {},
    ) =>
      this.request<Response, any>({
        path: `/flashcards/${id}`,
        method: "PUT",
        body: data,
        type: ContentType.Json,
        format: "json",
        ...params,
      }),
  };
  users = {
    /**
     * No description
     *
     * @tags Memoraid.WebApi
     * @name DeleteUser
     * @request DELETE:/users
     */
    deleteUser: (params: RequestParams = {}) =>
      this.request<Response, any>({
        path: `/users`,
        method: "DELETE",
        format: "json",
        ...params,
      }),
  };
}
