export function objectToQueryString(obj1?: any, obj2?: any, firstDelimiter: string = '?'): string {
  const str = getObjectQueryParameters(obj1).concat(getObjectQueryParameters(obj2));
  let ret = str.join('&');
  if ((ret.length > 0) && firstDelimiter) {
      ret = firstDelimiter + ret;
  }
  return ret;
}

function getObjectQueryParameters(obj?: any): string[] {
  const ret: string[] = [];
  if (obj) {
    for (const p in obj) {
      if (obj.hasOwnProperty(p) && (obj[p] !== undefined)) {
        if (Array.isArray(obj[p])) {
          for (const e of obj[p]) {
            if (e !== undefined) {
              ret.push(encodeURIComponent(p) + '=' + encodeURIComponent(e));
            }
          }
        } else {
          ret.push(encodeURIComponent(p) + '=' + encodeURIComponent(obj[p]));
        }
      }
    }
  }
  return ret;
}
