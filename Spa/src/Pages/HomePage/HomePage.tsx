import { useState } from 'react';
import { Box, Button, Stack, Typography } from '@mui/joy';
import { post } from '../../Services/ApiClient';
import FileUpload from "react-mui-fileuploader"

export default function HomePage(){

    const [files, setFiles] = useState([]);
    const handleChange = (files: []) => {
        setFiles([...files]);
    };

    const fileTypes = ["JPG", "PNG", "GIF"];

    const onUpload = () => {

        const formData = new FormData()
        files.forEach((file) => formData.append("images", file))
        
        post('api/documents/process/upload', formData)
    }

    return(
        <>
            <Box className='drag-n-drop' >
                {/* <FileUploader 
                    onFilesChange={handleChange} 
                    name="files" 
                    label="Выберите или перенесите файлы"
                    types={fileTypes} 
                    multiFile={true}
                    /> */}

                <FileUpload
                    onFilesChange={handleChange} 
                    multiFile
                    title={null}
                    header="Перенесите"
                    leftLabel="или"
                    buttonLabel="выберите"
                    rightLabel="файлы"
                    buttonRemoveLabel="Удалить все"
                    acceptedType={'image/*'}
                    allowedExtensions={fileTypes}
                />
            </Box>

            <Button onClick={onUpload}>
                Обработать
            </Button>

            <Box sx={{border: '1px solid #d0dae3', borderRadius: 8, mt: '10px'}}>
                <Typography px={1}>История</Typography>
                <Stack spacing={2}>
                    
                </Stack>
            </Box>
        </>
    )
}