import { Box, Stack, Table, Typography } from "@mui/joy";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import { get } from "../../Services/ApiClient";

export default function QueuePage() {

    const queryClient = useQueryClient();

    const { data } = useQuery<any[]>({
        queryKey: ['api/documents', 2],
        queryFn: () => get('api/documents', {
            params: {
                status: 1
            }
        })
    }, queryClient)

    return (
        <Box sx={{ border: '1px solid #d0dae3', borderRadius: 8 }}>
            {data &&
                <Table aria-label="basic table" hoverRow>
                    <caption>Не обработанные файлы</caption>
                    <thead>
                        <tr>
                            <th>Файл</th>
                        </tr>
                    </thead>
                    <tbody>
                        {
                            data.map((row) => (
                                <tr key={row.documentId}>
                                    <td>{row.fileName}</td>
                                </tr>
                            ))
                        }
                    </tbody>
                </Table>
            }
        </Box>
    )
}