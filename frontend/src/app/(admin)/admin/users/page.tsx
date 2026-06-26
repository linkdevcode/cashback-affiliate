import { UsersTable } from "@/features/admin/users/components/users-table";

export default function AdminUsersPage() {
  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-semibold tracking-tight">User management</h1>
        <p className="mt-2 text-sm text-muted-foreground">
          Search users, review account details, and manage active or suspended status.
        </p>
      </div>

      <UsersTable />
    </div>
  );
}
