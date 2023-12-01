import {ResponseData, ResponseJson} from "./interfaces";

let controller : AbortController;
export async function parseDataFromServer(value: number, ){
    if(controller) controller.abort();
    controller = new AbortController();
    const ans = await fetch(`/api/parse/${value.toString().replace('.',',')}`, {
        signal: controller.signal,
        method: 'GET',
        // body: JSON.stringify({value}),
        headers: {
            'Content-Type': 'application/json; charset=utf-8'
        }
    })
        .then(e=>e.json()) as ResponseJson<ResponseData>;
    console.assert(ans.success,"Request failed:" + ans.response.text)
    return ans.response.text;
}