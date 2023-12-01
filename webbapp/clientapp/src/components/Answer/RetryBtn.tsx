import {Button} from "@mui/material";

export function RetryBtn({onClick}: {onClick: ()=>any}){
    return (
        <Button variant="contained" onClick={onClick}>Retry</Button>
    )
}