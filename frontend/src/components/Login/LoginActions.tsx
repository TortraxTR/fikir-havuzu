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
                className="login-submit"
            >
                {loading ? 'Giriş yapılıyor...' : 'Giriş yap'}
            </Button>
            {error && <Alert className="login-error" severity="error">{error}</Alert>}
        </>
    );
}