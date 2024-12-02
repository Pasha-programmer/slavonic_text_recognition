import { Box, MenuItem, MenuList, Stack } from '@mui/joy';
import Typography from '@mui/joy/Typography';
import { RoutePaths } from '../Constants/RoutePaths';

export default function Menu(){

    return(
        <>
            <Box py={2}>
                <Typography>Система распознавания символов</Typography>
            </Box>
            <Box sx={{display: 'flex', height: 300}}>
                <Stack spacing={2}>
                    <MenuList>
                        <MenuItem component='a' href={RoutePaths.Documents}>Все документы</MenuItem>
                        <MenuItem component='a' href={RoutePaths.RecignizedDocuments}>Распознанные документы</MenuItem>
                        <MenuItem component='a' href={RoutePaths.Queue}>Очередь</MenuItem>
                    </MenuList>
                </Stack>
            </Box>
        </>
    )
}