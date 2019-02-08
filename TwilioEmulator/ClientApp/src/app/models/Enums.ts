export enum ApiCallDirection {
  ToEmulator = 0,
  FromEmulator = 1
}

export enum ApiCallType {
  IncomingCallCallback = 0,
  CallResourceApi = 1
}

export enum ResourceCudOperation {
  Reset = 0,
  Create = 1,
  Update = 2,
  Delete = 3,
}
