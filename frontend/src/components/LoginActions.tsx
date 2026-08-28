import ArrowForwardIcon from '@mui/icons-material/ArrowForward';
import { Alert, Button } from '@mui/material';

type LoginActionsProps = {
    loading: boolean;
    error: string | null;
};

export default function LoginActions({ loading, error }: LoginActionsProps) {
    return (
        <>
            <Button
                type="submit"
                fullWidth
                variant="contained"
                size="large"
                disabled={loading}
                endIcon={!loading && <ArrowForwardIcon />}
                sx={{ minHeight: 52, borderRadius: 1, bgcolor: '#d85c43', textTransform: 'none', fontWeight: 800, boxShadow: 'none', '&:hover': { bgcolor: '#bc4934', boxShadow: 'none' } }}
            >
                {loading ? 'Giriş yapılıyor...' : 'Giriş yap'}
            </Button>
            {error && <Alert severity="error" sx={{ mt: 2 }}>{error}</Alert>}
        </>
    );
}