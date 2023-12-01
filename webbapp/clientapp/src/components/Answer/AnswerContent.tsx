
import "./AnswerContent.scss"
import {useEffect, useState} from "react";
import {AnswerText} from "./AnswerText";
import {parseDataFromServer} from "../../api/downloader";
import {RetryBtn} from "./RetryBtn";

// Implementation with class:
// import React from "react";
// interface State {
//    error: boolean,
//    downloadedValue: string|null
// }
//
// export class AnswerContent extends React.Component<{value: number|null}, State>{
//     state = {error: false, downloadedValue: null}
//     loadData = ()=>{
//         if(this.props.value) {
//             this.setState({downloadedValue:null, error:false});
//             parseDataFromServer(this.props.value)
//                 .then((e)=> this.setState({downloadedValue:e}))
//                 .catch((e: any) => {
//                     // Request aborted because new sent
//                     if(e?.name === 'AbortError') return;
//                     this.setState({error:true})
//                     console.error(e);
//                 })
//         }
//         else this.setState({downloadedValue:""})
//     }
//     render(): React.ReactNode {
//         return (
//             <div className="answer">
//                 <AnswerText value={this.state.downloadedValue} isError={this.state.error}/>
//                 {
//                     this.state.error && <RetryBtn onClick={this.loadData} />
//                 }
//             </div>
//         );  
//     }
//     override componentDidUpdate(prevProps: Readonly<{ value: number | null; }>): void {
//         if (prevProps.value !== this.props.value){
//             this.loadData()
//         }                
//     }
// }

// Implementation with function
export function AnswerContent({value}: {value: number|null}){
    const [downloadedValue, setDownloadedValue] = useState<string|null>("");
    const [isError, setIsError] = useState<boolean>(false);
    useEffect(loadData, [value]);

    return (
        <div className="answer">
            <AnswerText value={downloadedValue} isError={isError}/>
            {
                isError && <RetryBtn onClick={loadData} />
            }
        </div>
    );

    function loadData(){
        if(value) {
            setDownloadedValue(null);
            setIsError(false);
            parseDataFromServer(value)
                .then(setDownloadedValue)
                .catch((e: any) => {
                    // Request aborted because new sent
                    if(e?.name === 'AbortError') return;
                    setIsError(true);
                    console.error(e);
                })
        }
        else setDownloadedValue("")
    }
}