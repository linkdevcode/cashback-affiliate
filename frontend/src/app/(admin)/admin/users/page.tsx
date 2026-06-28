import { UsersTable } from "@/features/admin/users/components/users-table";

export default function AdminUsersPage() {
  return (
    <div className="space-y-6">
      <p className="text-sm text-muted-foreground">
        Search users, review account details, and manage active or suspended status.
      </p>

      <UsersTable />
    </div>
  );
}
