docker build -t al-local/audit-listener-app .
kubectl run audit-listener-app-deployment --image=al-local/audit-listener-app:latest --image-pull-policy=Never --port 5253