import { FileUploader } from 'react-drag-drop-files';
import { useState } from 'react';
import { Box, Stack, Typography } from '@mui/joy';

export default function HomePage(){

    const fileTypes = ["JPG", "PNG", "GIF"];

    const [file, setFile] = useState<any>(null);
    const handleChange = (file: any) => {
        setFile(file);
    };

    return(
        <>
            <Box className='drag-n-drop' >
                <FileUploader handleChange={handleChange} 
                    name="file" 
                    label="Выберите или перенесите файл"
                    types={fileTypes} 
                    multiple 
                    />
            </Box>

            <Box sx={{border: '1px solid #d0dae3', borderRadius: 8, mt: '10px'}}>
                <Typography px={1}>История</Typography>
                <Stack spacing={2}>
                    
                </Stack>
            </Box>
        </>
    )
}