import { useState } from 'react'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from './components/ui/card'
import { Tabs, TabsContent, TabsList, TabsTrigger } from './components/ui/tabs'
import { Input } from './components/ui/input'
import { Button } from './components/ui/button'
import { Label } from './components/ui/label'
import { Alert, AlertDescription } from './components/ui/alert'
import { Users, Wallet, UserMinus } from 'lucide-react'
import type { CasinoUser, CreateUserRequest, UpdateBalanceResponse } from './types/api'

function App() {
  const [userId, setUserId] = useState<string>('')
  const [userForm, setUserForm] = useState<CreateUserRequest>({
    username: '',
    password: '',
    email: '',
    homePhoneNumber: '',
    workPhoneNumber: '',
    mobilePhoneNumber: ''
  })
  const [currentUser, setCurrentUser] = useState<CasinoUser | null>(null)
  const [balanceAmount, setBalanceAmount] = useState<string>('')
  const [message, setMessage] = useState<string>('')
  const [error, setError] = useState<string>('')

  const handleCreateUser = async () => {
    try {
      const response = await fetch(`${import.meta.env.VITE_API_URL}/api/user`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(userForm)
      })
      const data = await response.json()
      if (!response.ok) throw new Error(data.message || 'Failed to create user')
      setMessage('User created successfully!')
      setUserForm({
        username: '',
        password: '',
        email: '',
        homePhoneNumber: '',
        workPhoneNumber: '',
        mobilePhoneNumber: ''
      })
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to create user')
    }
  }

  const handleGetUser = async () => {
    try {
      const response = await fetch(`${import.meta.env.VITE_API_URL}/api/user/${userId}`)
      const data = await response.json()
      if (!response.ok) throw new Error(data.message || 'Failed to fetch user')
      setCurrentUser(data)
      setMessage('')
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to fetch user')
      setCurrentUser(null)
    }
  }

  const handleUpdateBalance = async () => {
    if (!currentUser?.userId) {
      setError('Please fetch a user first')
      return
    }
    try {
      const response = await fetch(`${import.meta.env.VITE_API_URL}/api/user/${currentUser.userId}/updateBalance?amount=${balanceAmount}`, {
        method: 'POST'
      })
      const data: UpdateBalanceResponse = await response.json()
      if (!response.ok) throw new Error(data.message || 'Failed to update balance')
      setCurrentUser(prev => prev ? { ...prev, balance: data.balance } : null)
      setMessage(`Balance updated successfully! New balance: $${data.balance}`)
      setBalanceAmount('')
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to update balance')
    }
  }

  const handleDeleteUser = async () => {
    if (!currentUser?.userId) {
      setError('Please fetch a user first')
      return
    }
    try {
      const response = await fetch(`${import.meta.env.VITE_API_URL}/api/user/${currentUser.userId}`, {
        method: 'DELETE'
      })
      if (!response.ok) {
        const data = await response.json()
        throw new Error(data.message || 'Failed to delete user')
      }
      setMessage('User deleted successfully!')
      setCurrentUser(null)
      setUserId('')
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to delete user')
    }
  }

  return (
    <div className="container mx-auto p-4 max-w-4xl">
      <h1 className="text-3xl font-bold mb-8 text-center">Casino User Management</h1>
      
      <Tabs defaultValue="create" className="space-y-4">
        <TabsList className="grid grid-cols-3 w-full">
          <TabsTrigger value="create" className="flex items-center gap-2">
            <Users className="h-4 w-4" /> Create User
          </TabsTrigger>
          <TabsTrigger value="manage" className="flex items-center gap-2">
            <Wallet className="h-4 w-4" /> Manage User
          </TabsTrigger>
          <TabsTrigger value="delete" className="flex items-center gap-2">
            <UserMinus className="h-4 w-4" /> Delete User
          </TabsTrigger>
        </TabsList>

        <TabsContent value="create">
          <Card>
            <CardHeader>
              <CardTitle>Create New User</CardTitle>
              <CardDescription>Create a new casino user account</CardDescription>
            </CardHeader>
            <CardContent className="space-y-4">
              <div className="grid grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label htmlFor="username">Username</Label>
                  <Input
                    id="username"
                    value={userForm.username}
                    onChange={e => setUserForm(prev => ({ ...prev, username: e.target.value }))}
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="password">Password</Label>
                  <Input
                    id="password"
                    type="password"
                    value={userForm.password}
                    onChange={e => setUserForm(prev => ({ ...prev, password: e.target.value }))}
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="email">Email</Label>
                  <Input
                    id="email"
                    type="email"
                    value={userForm.email}
                    onChange={e => setUserForm(prev => ({ ...prev, email: e.target.value }))}
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="homePhone">Home Phone</Label>
                  <Input
                    id="homePhone"
                    value={userForm.homePhoneNumber}
                    onChange={e => setUserForm(prev => ({ ...prev, homePhoneNumber: e.target.value }))}
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="workPhone">Work Phone</Label>
                  <Input
                    id="workPhone"
                    value={userForm.workPhoneNumber}
                    onChange={e => setUserForm(prev => ({ ...prev, workPhoneNumber: e.target.value }))}
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="mobilePhone">Mobile Phone</Label>
                  <Input
                    id="mobilePhone"
                    value={userForm.mobilePhoneNumber}
                    onChange={e => setUserForm(prev => ({ ...prev, mobilePhoneNumber: e.target.value }))}
                  />
                </div>
              </div>
              <Button onClick={handleCreateUser} className="w-full">Create User</Button>
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="manage">
          <Card>
            <CardHeader>
              <CardTitle>Manage User</CardTitle>
              <CardDescription>View and update user balance</CardDescription>
            </CardHeader>
            <CardContent className="space-y-4">
              <div className="flex gap-4">
                <div className="flex-1">
                  <Label htmlFor="userId">User ID</Label>
                  <Input
                    id="userId"
                    value={userId}
                    onChange={e => setUserId(e.target.value)}
                  />
                </div>
                <Button onClick={handleGetUser} className="mt-6">Fetch User</Button>
              </div>

              {currentUser && (
                <Card className="mt-4">
                  <CardHeader>
                    <CardTitle>User Details</CardTitle>
                  </CardHeader>
                  <CardContent>
                    <div className="grid grid-cols-2 gap-4">
                      <div>
                        <Label>Username</Label>
                        <p className="text-gray-600">{currentUser.username}</p>
                      </div>
                      <div>
                        <Label>Email</Label>
                        <p className="text-gray-600">{currentUser.email}</p>
                      </div>
                      <div>
                        <Label>Current Balance</Label>
                        <p className="text-gray-600">${currentUser.balance}</p>
                      </div>
                    </div>

                    <div className="mt-4 space-y-2">
                      <Label htmlFor="balanceAmount">Update Balance</Label>
                      <div className="flex gap-4">
                        <Input
                          id="balanceAmount"
                          type="number"
                          value={balanceAmount}
                          onChange={e => setBalanceAmount(e.target.value)}
                          placeholder="Enter amount"
                        />
                        <Button onClick={handleUpdateBalance}>Update</Button>
                      </div>
                    </div>
                  </CardContent>
                </Card>
              )}
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="delete">
          <Card>
            <CardHeader>
              <CardTitle>Delete User</CardTitle>
              <CardDescription>Permanently remove a user account</CardDescription>
            </CardHeader>
            <CardContent>
              {currentUser ? (
                <div className="space-y-4">
                  <Alert variant="destructive">
                    <AlertDescription>
                      Are you sure you want to delete user {currentUser.username}? This action cannot be undone.
                    </AlertDescription>
                  </Alert>
                  <Button variant="destructive" onClick={handleDeleteUser} className="w-full">
                    Delete User
                  </Button>
                </div>
              ) : (
                <p className="text-gray-600">Please fetch a user first using the Manage User tab.</p>
              )}
            </CardContent>
          </Card>
        </TabsContent>
      </Tabs>

      {message && (
        <Alert className="mt-4">
          <AlertDescription>{message}</AlertDescription>
        </Alert>
      )}

      {error && (
        <Alert variant="destructive" className="mt-4">
          <AlertDescription>{error}</AlertDescription>
        </Alert>
      )}
    </div>
  )
}

export default App
