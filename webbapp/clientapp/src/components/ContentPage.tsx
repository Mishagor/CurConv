import {NumberInput} from "./NumberInput";
import {useState} from "react";
import {AnswerContent} from "./Answer/AnswerContent";

export function ContentPage(){
    const [currentValue, setCurrentValue] = useState<number|null>(null);

    return (
        <div className="content">
            <NumberInput onChange={v=>setCurrentValue(v)}/>
            <AnswerContent value={currentValue}/>
        </div>
    )
}