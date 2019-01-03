import { ApiCallDirection, ApiCallType } from './Enums';

export class ApiCall {
  apiCallId?: string;
  sid?: string;
  direction?: ApiCallDirection;
  type?: ApiCallType;
  httpMethod?: string;
  url?: string;
  requestTimestamp?: Date;
  requestContentType?: string;
  requestContent?: string;
  responseTimestamp?: Date;
  responseContentType?: string;
  responseContent?: string;
  responseStatusCode?: number;
}
