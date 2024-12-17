import { FileUploader } from 'react-drag-drop-files';
import { useState } from 'react';
import { Box, Button, Stack, Typography } from '@mui/joy';
import { apiClient, post } from '../../Services/ApiClient';

export default function HomePage(){

    const [files, setFiles] = useState([]);
    const handleChange = (files: []) => {
        setFiles(files);
    };

    const fileTypes = ["JPG", "PNG", "GIF"];

    const onUpload = () => {

        const formData = new FormData()
        files.forEach((file) => formData.append("files", file))
        
        post('api/documents/process/upload', formData)
    }

    return(
        <>
            <Box className='drag-n-drop' >
                <FileUploader handleChange={handleChange} 
                    name="file" 
                    label="Выберите или перенесите файл"
                    types={fileTypes} 
                    multiFile 
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