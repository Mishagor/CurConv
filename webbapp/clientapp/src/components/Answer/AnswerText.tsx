import "./AnswerText.scss"
import {Loading} from "./Loading";
import {LoadedError} from "./LoadedError";
export function AnswerText({value, isError}: {value: string|null, isError: boolean}){
    return (
        <div>
            <b>Parsed text</b>:<br />
            {
                isError ?
                    <LoadedError /> :
                    <span className="answer-text">
                        {
                            (value === "" ? "Please enter the number." : (value?? <Loading />))
                        }
                    </span>
            }
        </div>
    )
}

