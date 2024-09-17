import { format } from 'date-fns';

export default function formatDate(date: Date): string {
    console.log(date)
    return format(date, 'dd/MM/yyyy HH:mm:ss'); 
}