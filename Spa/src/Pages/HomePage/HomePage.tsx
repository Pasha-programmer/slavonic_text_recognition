import { useState } from 'react';
import { Box, Button, Stack, Typography } from '@mui/joy';
import { post, get } from '../../Services/ApiClient';
import FileUpload from "react-mui-fileuploader"
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { endOfToday, startOfToday } from 'date-fns';
import Table from '@mui/joy/Table';

export default function HomePage(){

    const [files, setFiles] = useState([]);
    const handleChange = (files: []) => {
        setFiles([...files]);
    };

    const queryClient = useQueryClient();

    const upload = useMutation({
        mutationKey: ['api/documents/process/upload'],
        mutationFn: (formData: FormData) => post('api/documents/process/upload', formData),
        onSuccess: () => {
            debugger
            queryClient.invalidateQueries({
                queryKey: ['api/documents']
            })
            setFiles([]);
        }
    }, queryClient)

    const { data } = useQuery<any[]>({
        queryKey: ['api/documents', startOfToday(), endOfToday()],
        queryFn: () => get('api/documents', {
            params: {
                fromDate: startOfToday(),
                toDate: endOfToday(),
            }
        })
    }, queryClient)

    const fileTypes = ["JPG", "PNG", "GIF"];

    const onUpload = async () => {

        const formData = new FormData()
        files.forEach((file) => formData.append("images", file))

        upload.mutate(formData)
    }

    return(
        <>
            <Box className='drag-n-drop' >
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
                <div className='actions'>

                    <Button onClick={onUpload}>
                        Обработать
                    </Button>
                </div>
            </Box>

            <Box sx={{border: '1px solid #d0dae3', borderRadius: 8, mt: '10px'}}>
                <Stack spacing={2}>
                    {data &&
                        <Table aria-label="basic table" hoverRow>
                            <caption>История</caption>
                            <thead>
                                <tr>
                                    <th>Файл</th>
                                    <th>Текст</th>
                                </tr>
                            </thead>
                            <tbody>
                                {
                                    data.map((row) => (
                                        <tr key={row.documentId}>
                                            <td>{row.fileName}</td>
                                            <td>{row.content}</td>
                                        </tr>
                                    ))
                                }
                            </tbody>
                        </Table>
                    }
                </Stack>
            </Box>
        </>
    )
}